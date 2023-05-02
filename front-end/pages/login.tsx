import axios from "axios"
import {
    TextInput,
    PasswordInput,
    Checkbox,
    Anchor,
    Paper,
    Title,
    Text,
    Container,
    Group,
    Button,
} from '@mantine/core';
import { useState } from "react";

export default function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [cookeie, setCookie] = useState("");

    function login() {
        axios.post('https://localhost:7280/api/auth/login', {
            email,
            password
        })
            .then(res => {
                console.log( res.data["access_token"])

                document.cookie = res.data["access_token"];
                console.log(document.cookie)

                // const receivedCookie = res.data["access_token"];
                // setCookie(receivedCookie);
            })
            .catch(error => console.log(error));
    }

    return (
        <>
            <Container size={420} my={40}>
                <Title align="center" sx={(theme) => ({ fontFamily: `Greycliff CF, ${theme.fontFamily}`, fontWeight: 900 })}>
                    Tender
                </Title>
                <Text color="dimmed" size="sm" align="center" mt={5}>
                    Do not have an account yet?{' '}
                    <Anchor size="sm" component="button">
                        Create account
                    </Anchor>
                </Text>

                <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                    <TextInput value={email} onChange={(event) => setEmail(event.target.value)} label="Email" placeholder="something@gmail.com" required />
                    <PasswordInput value={password} onChange={(event) => setPassword(event.target.value)} label="Password" placeholder="pass123" required mt="md" />
                    <br />
                    <Button fullWidth mt="x1" onClick={login}>
                        Sign in
                    </Button>
                </Paper>
            </Container>
        </>
    )
}