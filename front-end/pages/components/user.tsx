import axios from "axios";
import { useEffect, useState } from "react";
import { Card, Avatar, Text, Button, Paper, Image } from '@mantine/core';

export default function User({ props }: { props: any }) {
    const [images, setImages] = useState([]);
    const [imageNames, setImageNames] = useState([]);

    useEffect(() => {
        axios.get(`https://localhost:7282/api/profile/${props.userId}`)
            .then(res => {
                console.log(res.data)
                setImages(res.data['imageUrl'])
                setImageNames(res.data['imageName'])
            })
            .catch(error => console.log(error))
    }, [])

    return (
        <>
            <Paper
                radius="md"
                withBorder
                p="lg"
            >
                <Avatar src={images[1]} size={120} radius={120} mx="auto" />
                <Text ta="center" fz="lg" weight={500} mt="md">
                    {props?.firstName} {props?.lastName}
                </Text>

                <Button variant="default" fullWidth mt="md">
                    Like
                </Button>


                <div style={{ marginTop: "10px", display: "flex" }} >
                    {images.map((p, index) => {
                        return (
                            <>
                                <Card style={{ inlineSize: "fit-content", width: "40%" }} key={imageNames[index]}>
                                    <Card.Section>
                                        <Image src={p} />
                                    </Card.Section>
                                </Card>
                            </>
                        )
                    })}
                </div>
            </Paper>
        </>
    )
}