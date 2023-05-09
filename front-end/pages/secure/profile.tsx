import { Avatar, Text, Button, Paper } from '@mantine/core';
import Navbar from '../components/navbar';

interface UserInfoProps {
    avatar: string;
    email: string;
    firstName: string;
    lastName: string;
    pictures: [];
}

export default function Profile({ avatar, email, firstName, lastName, pictures }: UserInfoProps) {
    return (
        <>
            <Navbar/>
            <Paper
                radius="md"
                withBorder
                p="lg"
            >
                <Avatar src={avatar} size={120} radius={120} mx="auto" />
                <Text ta="center" fz="lg" weight={500} mt="md">
                    {firstName} {lastName}
                </Text>

                <Button variant="default" fullWidth mt="md">
                    Edit Info
                </Button>
            </Paper>
        </>
    )
}