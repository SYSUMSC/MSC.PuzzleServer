import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export interface User {
  userName: string;
  description: string;
  studentId: string;
  realName: string;
  isSYSU: boolean;
  phoneNumber: string;
  email: string;
}

export interface UserLoginDto {
  userName: string;
  password: string;
  gToken: string;
}

export const USER_API = createApi({
  reducerPath: 'userApi',
  baseQuery: fetchBaseQuery({ baseUrl: 'Account' }),
  endpoints: (builder) => ({
    status: builder.query<User, void>({
      query: () => 'Me'
    }),
    login: builder.query<void, UserLoginDto>({
      query: (dto) => ({
        url: 'LogIn',
        method: 'POST',
        body: dto
      })
    })
  })
});
