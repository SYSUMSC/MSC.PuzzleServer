import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export interface Submission {
  id: number;
  answer: string;
  solved: boolean;
  score: number;
  submitTimeUTC: string;
}

export const SUBMISSION_API = createApi({
  reducerPath: 'submissionApi',
  baseQuery: fetchBaseQuery({ baseUrl: 'api/submission' }),
  endpoints: (builder) => ({
    getLatestSubmissions: builder.query<Submission[], number>({
      query: (id) => `${id}`
    }),
    getLatestSubmissionsOfAllUsers: builder.query<Submission[], number>({
      query: (id) => `history/${id}`
    })
  })
});
