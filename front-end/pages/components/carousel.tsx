import { Card, Image, Text, Button, Group } from '@mantine/core';
import { Carousel } from '@mantine/carousel';

const images = [
    'https://images.unsplash.com/photo-1598928506311-c55ded91a20c?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=720&q=80',
    'https://images.unsplash.com/photo-1567767292278-a4f21aa2d36e?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=720&q=80',
    'https://images.unsplash.com/photo-1605774337664-7a846e9cdf17?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=720&q=80',
    'https://images.unsplash.com/photo-1554995207-c18c203602cb?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=720&q=80',
    'https://images.unsplash.com/photo-1616486029423-aaa4789e8c9a?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=720&q=80',
]

export default function CarouselComponent() {

    const slides = images.map((image) => (
        <Carousel.Slide key={image}>
            <Image src={image} height={220} />
        </Carousel.Slide>
    ))

    return (
        <Card shadow="sm" padding="lg" radius="md" withBorder style={{ width: "40%" }}>
            <Card.Section>
                <Carousel>{slides}</Carousel>
            </Card.Section>

            <Group position="apart" mt="md" mb="xs">
                <Text weight={500}>John Doe</Text>
            </Group>

            <Button variant="light" color="pink" fullWidth mt="md" radius="md">
                Like
            </Button>
        </Card>
    );
}