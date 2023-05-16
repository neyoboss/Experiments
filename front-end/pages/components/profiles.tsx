import { Avatar, Text, Button, Paper } from '@mantine/core';
interface UserInfoActionProps {
    avatar: string;
    name: string;
    email: string;
    job: string;
}

export default function Profiles({ avatar, name, email, job }: UserInfoActionProps) {
    return (
        <Paper
            radius="md"
            withBorder
            p="lg"
            style={{width:'30%'}}
        >
            <Avatar src={avatar} size={120} radius={120} mx="auto" />
            <Text ta="center" fz="lg" weight={500} mt="md">
                {name}
            </Text>
            <Text ta="center" c="dimmed" fz="sm">
                {email} • {job}
            </Text>

            <Button variant="default" fullWidth mt="md">
                Send message
            </Button>
        </Paper>
    );
}