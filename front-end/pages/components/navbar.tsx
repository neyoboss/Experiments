import { Button } from '@mantine/core';
import axios from 'axios';
import Link from 'next/link';

export default function Navbar() {

  const logout = async () => {
    await axios.post('https://localhost:7280/api/auth/logout', {}, {
      headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Credentials': true
      },
      withCredentials: true
    })
      .then(res => {
        // clear user data from context and localStorage
        // setUser(null);
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