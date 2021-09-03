import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export interface PuzzleLog {
  time: string;
  userName: string;
  ip: string;
  msg: string;
  status: string;
}

export const ADMIN_API = createApi({
  reducerPath: 'adminApi',
  baseQuery: fetchBaseQuery({ baseUrl: 'Admin' }),
  endpoints: (builder) => ({
    getLogs: builder.query<PuzzleLog[], void>({
      query: () => 'Logs'
    })
  })
});
