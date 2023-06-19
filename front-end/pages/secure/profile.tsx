import { Card, Avatar, Text, Button, Paper, Image } from '@mantine/core';
import { useContext, useEffect, useState } from 'react';
import axios from 'axios';
import Navbar from '../components/navbar';
import { useAtom } from 'jotai';
import {userAtom}  from '../../utils/userAtom';


export default function Profile() {

    const [images, setImages] = useState([]);
    const [imageNames, setImageNames] = useState([]);


    const [imageBlob, setImageBlob] = useState<Blob | null>(null);
    const [responseMessage, setResponseMessage] = useState("");

    const [user] = useAtom<any>(userAtom)

    // if (user === null) {
    //     return <div>Banica</div>;
    // }


    useEffect(() => {
        axios.get(`https://localhost:7282/api/profile/${user.userId}`)
            .then(res => {
                console.log(res.data)
                setImages(res.data['imageUrl'])
                setImageNames(res.data['imageName'])
            })
            .catch(error => console.log(error))
    }, [])

    async function handleFileUpload(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        if (imageBlob) {
            const formData = new FormData();
            formData.append("image", imageBlob);
            formData.append("userId", user.userId)
            console.log(imageBlob.size)

            try {
                const response = await fetch("https://localhost:7282/api/profile/uploadImage", {
                    method: "POST",
                    body: formData
                });

                const data = await response.json();

                setResponseMessage(data.downloadURL);
            } catch (error) {
                console.error(error);
            }
        }
    }

    async function handleFileChange(event: React.ChangeEvent<HTMLInputElement>) {
        event.preventDefault();
        setResponseMessage("");
        if (!event.target.files) throw new Error("No file selected");
        const file = event.target.files[0];

        if (!file) throw new Error("No file selected");

        const imageBlob = new Blob([await file.arrayBuffer()], {
            type: file?.type,
        });

        setImageBlob(imageBlob);
    }

    async function deleteImage(userId, imageName) {
        await axios.delete("https://localhost:7282/api/profile/deleteImage", {
            params: {
                userId,
                imageName
            }
        })
    }



    return (
        <>
            <Navbar />

            <Paper
                radius="md"
                withBorder
                p="lg"
            >
                <Avatar src={images[1]} size={120} radius={120} mx="auto" />
                <Text ta="center" fz="lg" weight={500} mt="md">
                    {user ? (<>{user.email}</>) : (<><h1>No user</h1></>)}
                </Text>

                <Button variant="default" fullWidth mt="md">
                    Edit Info
                </Button>

                <div>
                    <form className="flex w-full gap-4" onSubmit={handleFileUpload}>
                        <label className="btn inline-flex w-96 gap-2" htmlFor="upload-file">

                            Choose File
                            <input
                                id="upload-file"
                                className="hidden"
                                type="file"
                                accept="image/png, image/jpeg"
                                onChange={handleFileChange}
                            />
                        </label>

                        <Button className="btn w-full" type="submit">
                            Upload
                        </Button>
                    </form>
                    {responseMessage && (
                        <div className="mt-2 border-2">
                            <code>{`![alt](${responseMessage})`}</code>
                        </div>
                    )}
                </div>

                <div style={{ marginTop: "10px", display: "flex" }} >
                    {images.map((p, index) => {
                        return (
                            <>
                                <Card style={{ inlineSize: "fit-content", width: "40%" }} key={imageNames[index]}>
                                    <Card.Section>
                                        <Image src={p} />
                                    </Card.Section>
                                    <Button onClick={() => deleteImage(user.userId, imageNames[index])} color="red" fullWidth mt="md" radius="md">
                                        Delete
                                    </Button>
                                </Card>
                            </>
                        )
                    })}
                </div>
            </Paper>
        </>
    )
}