import { Inter } from 'next/font/google'
import Navbar from './components/navbar';

const inter = Inter({ subsets: ['latin'] })

export default function Home() {

  return (
    <>
    <Navbar />
      <h1>Home</h1>
    </>
  );
}
