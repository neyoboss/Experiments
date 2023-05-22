import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

// This function can be marked `async` if using `await` inside
export function middleware(request: NextRequest) {
    // if (request.nextUrl.pathname.startsWith('/secure')) {
    //     const { cookies } = request;
    //     if (!cookies.get('access_token')) {
    //         const response = NextResponse.redirect(new URL('/login', request.url));
    //         return response;
    //     }
    // }

    // const response = NextResponse.next();
    // return response;

}
