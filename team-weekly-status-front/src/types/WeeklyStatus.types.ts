export interface WeeklyStatusData {
    id: number;
    weekStartDate: Date | string; 
    doneThisWeek: string[];
    planForNextWeek: string[];
    upcomingPTO: (Date | string)[]; 
    blockers: string;
    memberId: number;
}


export interface UserValidationResult {
    success: boolean;
    role: string;
    teamName: string;
    memberId: number;
    memberName: string;
}
