import '@/styles/globals.css'
import type { AppProps } from 'next/app'
import { MantineProvider } from '@mantine/core';
import { UserProvider } from '@auth0/nextjs-auth0/client';


export default function App({ Component, pageProps }: AppProps) {
  return (
    <>
      <UserProvider>
        <MantineProvider
          withGlobalStyles
          withNormalizeCSS
          theme={{
            /** Put your mantine theme override here */
            colorScheme: 'light',
          }}
        >
          <Component {...pageProps} />
        </MantineProvider>
      </UserProvider>
    </>
  )
};
