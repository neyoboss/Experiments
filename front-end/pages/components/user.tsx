import axios from "axios";
import { use, useEffect, useState } from "react";
import { Card, Avatar, Text, Button, Paper, Image } from '@mantine/core';

export default function User({ props }: { props: any }) {
    const [images, setImages] = useState([]);
    const [imageNames, setImageNames] = useState([]);
    const [userView, setUserView] = useState();
    
    console.log(props.id)

    useEffect(() => {
        axios.get(`https://localhost:7282/api/profile/${props.id}`)
            .then(res => {
                console.log(res.data)
                setImages(res.data['imageUrl'])
                setImageNames(res.data['imageName'])
                setUserView(res.data)
                console.log()
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
                                <Card  key={imageNames[index]} style={{ inlineSize: "fit-content", width: "40%" }}>
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