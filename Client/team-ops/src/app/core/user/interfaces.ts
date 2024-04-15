export interface SignInResponse {
  token: string;
  user: User;
}

export interface SignInRequest {
  usernameOrEmail: string;
  password: string;
}

export interface User {
  id: string;
  email: string;
  name: string;
  surname: string;
  username: string;
  token: string;
  gitHubUser:string
}

export type UserAuthState = {
  isAuthenticated: boolean;
  user: User | null;
  token: string | null;
};

export type SignUpRequest = {
  name: string;
  surname: string;
  username: string;
  email: string;
  pasword: string;
  inviteeId:number;
  gitHubUser: string
};

export type SignUpResponse = {
  user: User;
};
