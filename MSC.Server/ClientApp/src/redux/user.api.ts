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

export interface UserRegisterDto {
  userName: string;
  password: string;
  email: string;
}

export interface UserLoginDto {
  userName: string;
  password: string;
}

export interface UserRecoveryDto {
  email: string;
}

export interface UserResetPasswordDto {
  password: string;
  email: string;
  rToken: string;
}

export interface UserVerifyEmailDto {
  token: string;
  email: string;
}

export interface UserUpdateInfoDto {
  userName: string;
  descr: string;
  studentId: string;
  phoneNumber: string;
  realName: string;
}

export interface UserChangePasswordDto {
  old: string;
  new: string;
}

export interface UserChangeEmailDto {
  newMail: string;
}

export interface UserConfirmChangingEmailDto {
  token: string;
  email: string;
}

export const USER_API = createApi({
  reducerPath: 'userApi',
  baseQuery: fetchBaseQuery({ baseUrl: 'Account' }),
  endpoints: (builder) => ({
    status: builder.query<User, void>({
      query: () => 'Me'
    }),
    register: builder.query<void, UserRegisterDto>({
      query: (dto) => ({
        url: 'Register',
        method: 'POST',
        body: dto
      })
    }),
    login: builder.query<void, UserLoginDto>({
      query: (dto) => ({
        url: 'LogIn',
        method: 'POST',
        body: dto
      })
    }),
    recovery: builder.query<void, UserRecoveryDto>({
      query: (dto) => ({
        url: 'Recovery',
        method: 'POST',
        body: dto
      })
    }),
    resetPassword: builder.query<void, UserResetPasswordDto>({
      query: (dto) => ({
        url: 'PasswordReset',
        method: 'POST',
        body: dto
      })
    }),
    verifyEmail: builder.query<void, UserVerifyEmailDto>({
      query: (dto) => ({
        url: 'Verify',
        method: 'POST',
        body: dto
      })
    }),
    logout: builder.query<void, void>({
      query: () => ({
        url: 'LogOut',
        method: 'POST'
      })
    }),
    updateInfo: builder.query<void, UserUpdateInfoDto>({
      query: (dto) => ({
        url: 'Update',
        method: 'PUT',
        body: dto
      })
    }),
    changePassword: builder.query<void, UserChangePasswordDto>({
      query: (dto) => ({
        url: 'Update',
        method: 'PUT',
        body: dto
      })
    }),
    changeEmail: builder.query<void, UserChangeEmailDto>({
      query: (dto) => ({
        url: 'ChangeEmail',
        method: 'PUT',
        body: dto
      })
    }),
    confirmChangingEmail: builder.query<void, UserConfirmChangingEmailDto>({
      query: (dto) => ({
        url: 'MailChangeConfirm',
        method: 'POST',
        body: dto
      })
    })
  })
});
