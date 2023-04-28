import { Inter } from 'next/font/google'
import Link from 'next/link'
import { useUser } from '@auth0/nextjs-auth0/client';

const inter = Inter({ subsets: ['latin'] })

export default function Home() {
  // const { user, isLoading, error } = useUser();

  // if (isLoading) {
  //   return <div>Loading...</div>;
  // }

  // if (error) {
  //   return <div>Error: {error.message}</div>;
  // }

  // return (
  //   <div>
  //     {user ? (
  //       <div>
  //         <h1>Welcome, {user.name}</h1>
  //         <button onClick={() => fetch('/api/auth/logout')}>Log Out</button>
  //       </div>
  //     ) : (
  //       <button onClick={() => fetch('/api/auth/login')}>Log In</button>
  //     )}
  //   </div>
  // );
  return <a href="/api/auth/login">Login</a>;
}
