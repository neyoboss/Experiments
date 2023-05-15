import '@/styles/globals.css'
import type { AppProps } from 'next/app'
import { MantineProvider } from '@mantine/core';
import Navbar from './components/navbar';
import { UserProvider } from './userContext';

export default function App({ Component, pageProps: { session, ...pageProps } }: AppProps) {
  return (
    <>
      <MantineProvider
        withGlobalStyles
        withNormalizeCSS
        theme={{
          /** Put your mantine theme override here */
          colorScheme: 'light',
        }}
      >
        <UserProvider>
          <Navbar />
          <Component {...pageProps} />
        </UserProvider>
      </MantineProvider>
    </>
  )
};
