export interface ProfileResponse {
  id: string;
  email: string;
  fullName: string;
  level: string;
  createdAt: string;
}

export interface ProfileRequest {
  fullName: string;
}
