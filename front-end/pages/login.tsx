import axios from "axios"
import {
    TextInput,
    PasswordInput,
    Anchor,
    Paper,
    Title,
    Text,
    Container,
    Button,
} from '@mantine/core';
import { useContext, useState } from "react";
import Link from "next/link";
import { useRouter } from "next/router";
import  {userAtom}  from "../utils/userAtom";
import { useAtom } from "jotai";

export default function Login() {
    
    const router = useRouter();

    const [user, setUser] = useAtom(userAtom);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    function login() {
        axios.post('https://localhost:7280/api/auth/login', {
            email,
            password
        }, {
            headers: {
                'Content-Type': 'application/json',
                'Access-Control-Allow-Credentials': true
            },
            withCredentials: true
        })
            .then(res => {
                console.log(res.data['user'])
                const userData = res.data['user'];
                setUser(userData);
                localStorage.setItem('user', JSON.stringify(userData));
                router.push("/");
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
                        <Link href="/register">Create account</Link>
                    </Anchor>
                </Text>

                <Paper withBorder shadow="md" p={30} mt={30} radius="md">
                    <TextInput value={email} onChange={(event) => setEmail(event.target.value)} label="Email" placeholder="something@gmail.com" required />
                    <PasswordInput value={password} onChange={(event) => setPassword(event.target.value)} label="Password" placeholder="asd123ASD!" required mt="md" />
                    <br />
                    <Button fullWidth mt="x1" onClick={login}>
                        Sign in
                    </Button>
                </Paper>
            </Container>
        </>
    )
}