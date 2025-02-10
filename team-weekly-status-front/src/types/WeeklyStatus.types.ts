export interface Subtask {
  subtaskDescription: string;
}
export interface TaskWithSubtasks {
  taskDescription: string;
  subtasks: Subtask[];
}
export interface WeeklyStatusData {
  id: number;
  weekStartDate: Date | string;
  doneThisWeek: TaskWithSubtasks[];
  planForNextWeek: TaskWithSubtasks[];
  upcomingPTO: (Date | string)[];
  blockers: string;
  memberId: number;
  teamId: number;
}

export interface TeamMemberWeeklyStatusData {
  memberName: string;
  weeklyStatus: WeeklyStatusData | null;
}

export type TeamWeeklyStatusData = TeamMemberWeeklyStatusData[];

export interface UserValidationResult {
  success: boolean;
  role: string;
  teamName: string;
  memberId: number;
  memberName: string;
}

export type Member = {
  id: number;
  name: string;
  email?: string;
};

export interface GoogleLoginResponse {
  success: boolean;
  memberId: number | 0;
  memberName: string | "";
  isAdmin: boolean;
}

export interface AIEngine {
  aiEngineId: number;
  aiEngineName: string;
}
export interface TeamAIConfigurationRead {
  aiEngineid?: number;
  aiEngineName?: string;
  apiUrl?: string;
  apiKey?: string;
  model?: string;
  aiEngine?: AIEngine;
}

export type TeamRead = {
  id: number;
  name: string;
  description?: string;
  emailNotificationsEnabled?: boolean;
  slackNotificationsEnabled?: boolean;
  isActive: boolean;
  weekReporterAutomaticAssignment?: boolean;
  aiConfiguration?: TeamAIConfigurationRead;
};

export interface TeamAIConfiguration {
  aiEngineid?: number;
  aiEngineName?: string;
  apiUrl?: string;
  apiKey?: string;
  model?: string;
}

export type Team = {
  id: number;
  name: string;
  description?: string;
  emailNotificationsEnabled?: boolean;
  slackNotificationsEnabled?: boolean;
  isActive: boolean;
  weekReporterAutomaticAssignment?: boolean;
  aiConfiguration?: TeamAIConfiguration;
};

export type TeamMember = {
  teamId: number;
  teamName: string;
  memberId: number;
  memberName: string;
  isTeamLead: boolean;
  isCurrentWeekReporter: boolean;
  startActiveDate: string;
  endActiveDate: string;
};

export type MemberTeams = TeamMember[];

export type UserMember = {
  id: number;
  name: string;
  email: string;
  isAdmin?: boolean;
};

export type Reporter = {
  memberId: number;
  memberName: string;
  email?: string;
};

export interface JungleLoginResponse {
  memberId: number | 0;
  memberName: string | "";
  jwtToken: string;
  isAdmin: boolean;
}

export interface SupportContact {
  name: string;
  email: string;
}

export interface UserProvisioningResponse {
  message: string;
  contactsNotified: SupportContact[];
}

// Union type for authentication responses
export type AuthResponse =
  | JungleLoginResponse
  | GoogleLoginResponse
  | UserProvisioningResponse;

export interface WeeklyStatusRichTextData {
  id: number;
  weekStartDate: Date | string;
  doneThisWeekContent: string;
  planForNextWeekContent: string;
  upcomingPTO: (Date | string)[];
  blockers: string;
  memberId: number;
  teamId: number;
}

export interface TeamMemberWeeklyStatusRichTextData {
  memberName: string;
  weeklyStatus: WeeklyStatusRichTextData | null;
}


export type TeamWeeklyRichTextStatusData = TeamMemberWeeklyStatusRichTextData[];


export interface TeamAIConfigurationResponse {
  success: boolean;
  teamAIConfiguration: TeamAIConfiguration;
}

export interface TeamAIConfigurationRequest {
  aiEngineName: string;
  apiUrl: string;
  apiKey: string;
  model: string;
}
