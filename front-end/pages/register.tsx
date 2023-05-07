import axios from "axios"
import {
    TextInput,
    PasswordInput,
    Paper,
    Title,
    Text,
    Container,
    Button,
    Box,
    Progress,
    Group,
    Center
} from '@mantine/core';
import { useState } from "react";

export default function Register() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");

    function register() {
        axios.post('https://localhost:7280/api/auth/register', {
            email,
            firstName,
            lastName,
            password
        })
            .then(res => {
                console.log(res.data)
            })
            .catch(error => console.log(error));
    }

    return (
        <>
            <Container size={420} my={40}>
                <Title align="center" sx={(theme) => ({ fontFamily: `Greycliff CF, ${theme.fontFamily}`, fontWeight: 900 })}>
                    Register Tender
                </Title>

                <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                    <TextInput value={email} onChange={(event) => setEmail(event.target.value)} label="Email" placeholder="something@gmail.com" required />
                    <TextInput value={firstName} onChange={(event) => setFirstName(event.target.value)} label="First Name" placeholder="John" required />
                    <TextInput value={lastName} onChange={(event) => setLastName(event.target.value)} label="Last Name" placeholder="Doe" required />
                    <PasswordInput value={password} onChange={(event) => setPassword(event.target.value)} label="Password" placeholder="pass123" required mt="md" />
                    <br />
                    <Button fullWidth mt="x1" onClick={register}>
                        Register
                    </Button>
                </Paper>
            </Container>
        </>
    )
}