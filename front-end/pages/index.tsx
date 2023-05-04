import { Inter } from 'next/font/google'
import Link from 'next/link'
import { useUser } from '@auth0/nextjs-auth0/client';
import { useEffect } from 'react';
import axios from 'axios';
import { getAccessToken } from '@auth0/nextjs-auth0';

const inter = Inter({ subsets: ['latin'] })

export default function Home() {

  return (
    <>
        <a href='/api/auth/login'>Login</a>
    </>
  );
}
