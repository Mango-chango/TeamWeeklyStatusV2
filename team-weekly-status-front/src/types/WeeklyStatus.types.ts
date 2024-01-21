import WeeklyStatus from "../components/WeeklyStatus/index";
export interface TaskWithSubtasks {
  taskDescription: string;
  subtasks: string[];
}
export interface WeeklyStatusData {
  id: number;
  weekStartDate: Date | string;
  doneThisWeek: TaskWithSubtasks[];
  planForNextWeek: string[];
  upcomingPTO: (Date | string)[];
  blockers: string;
  memberId: number;
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
  role: "TeamLead" | "CurrentWeekReporter" | "Normal" | null;
  teamName: string | "";
  memberId: number | 0;
  memberName: string | "";
}
