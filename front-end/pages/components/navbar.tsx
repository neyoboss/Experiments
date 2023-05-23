import { Button } from '@mantine/core';
import axios from 'axios';
import Link from 'next/link';
import { useAtom } from 'jotai';
import  {userAtom}  from '../../utils/userAtom';
import { useRouter } from 'next/router';


export default function Navbar() {
  const router = useRouter();

  const logout = async () => {
    await axios.post('https://localhost:7280/api/auth/logout', {}, {
      headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Credentials': true
      },
      withCredentials: true
    })
      .then(res => {
        router.push('../login');
        localStorage.removeItem("user");
      })
      .catch(error => console.log(error));
  }

  return (
    <>
      <ul>
        <li>
          <Button onClick={logout}>
            <Link href="/">
              Logout
            </Link>
          </Button>
        </li>
        <li>
          <Button>
            <Link href="/secure/profile">
              Profile
            </Link>
          </Button>
        </li>
        <li>
          <Button>
            <Link href="/">
              Home
            </Link>
          </Button>
        </li>
      </ul>
    </>
  );
}