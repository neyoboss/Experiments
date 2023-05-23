import Navbar from './components/navbar';
import { useEffect, useState } from 'react';
import axios from 'axios';
import { useAtom } from 'jotai';
import {userAtom}  from '../utils/userAtom';
import { Card, Text, Button, Group, Modal } from '@mantine/core';
import { useDisclosure } from '@mantine/hooks';
import User from './components/user';

export default function Home() {
  const [users, setUsers] = useState<any>([]);
  const [user] = useAtom<any>(userAtom);
  const [opened, { open, close }] = useDisclosure(false);


  // if (user === null) {
  //   return <div>Banica</div>;
  // }



  // async function like() {
  //   await axios.post("")
  // }


  useEffect(() => {
    async function fetchData() {
      try {
        const response = await axios.get("https://localhost:7282/api/profile/getProfilesWithoutCurrent", {
          headers: {
            userId: user.userId
          }
        });
  
        setUsers(response.data);
        console.log(response.data);
      } catch (error) {
        console.log(error);
      }
    }
  
    fetchData();
  }, [])

  return (
    <>
      <Navbar />
      <h1>Home</h1>

      <div>
        {users.map((u) => {
          return (
            <>
              <Card shadow="sm" padding="lg" radius="md" withBorder key={u.userId}>

                <div style={{ display: "flex", flexDirection: "row", justifyContent: "space-between" }}>
                  <Group position="apart" mt="md" mb="xs">
                    <Text weight={500}>{u.firstName} {u.lastName}</Text>
                  </Group>

                  <Modal opened={opened} onClose={close}>
                   {user? <User props={user} /> : <div>Nema</div>} 
                  </Modal>

                  <Button onClick={open} variant="light" color="blue" mt="md" radius="md">
                    View
                  </Button>
                </div>
              </Card>
            </>
          )
        })}
      </div>
    </>
  );
}
