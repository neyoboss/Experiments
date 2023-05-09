import { Button } from '@mantine/core';
import axios from 'axios';

export default function Navbar(){

  const logout = async () =>{
    await axios.post("logout url").catch(error => console.log(error))
    localStorage.clear()
  }

    return (
        <>
          <ul>
            <li><Button onClick={logout}><a href="">Logout</a></Button></li>
            <li><Button><a href="/secure/profile">Profile</a></Button></li>
            <li><Button><a href="/">Home</a></Button></li>
          </ul>
        </>
      );
}