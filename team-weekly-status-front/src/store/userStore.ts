import { create } from 'zustand';
import { MemberTeams } from '../types/WeeklyStatus.types';

type UserState = {
  teamId: number;
  teamName: string | null;
  memberId: number;
  memberName: string | null;
  isAuthenticated: boolean;
  memberActiveTeams: MemberTeams | null;
  isAdmin: boolean;
  isTeamLead: boolean;
  isCurrentWeekReporter: boolean;
  //setRole: (role: 'TeamLead' | 'CurrentWeekReporter' | 'Normal' | null) => void;
  setTeamId: (teamId: number) => void;
  setTeamName: (teamName: string) => void;
  setMemberId: (memberId: number) => void;
  setMemberName: (memberName: string) => void;
  setIsAuthenticated: (isAuthenticated: boolean) => void;
  setMemberActiveTeams: (memberTeams: MemberTeams) => void;
  setIsAdmin: (isAdmin: boolean) => void;
  setIsTeamLead: (isTeamLead: boolean) => void;
  setIsCurrentWeekReporter: (isCurrentWeekReporter: boolean) => void;
}

const userStore = create<UserState>((set) => ({
  teamId: 0,
  setTeamId: (teamId: number) => set({ teamId }),
  teamName: null,
  setTeamName: (teamName: string) => set({ teamName }),
  memberId: 0,
  setMemberId: (memberId: number) => set({ memberId }),
  memberName: null,
  setMemberName: (memberName: string) => set({ memberName }),
  isAuthenticated: false,
  setIsAuthenticated: (isAuthenticated: boolean) => set({ isAuthenticated }),
  memberActiveTeams: null,
  setMemberActiveTeams: (memberActiveTeams: MemberTeams) => set({ memberActiveTeams }),
  isAdmin: false,
  setIsAdmin: (isAdmin: boolean) => set({ isAdmin }),
  isTeamLead: false,
  setIsTeamLead: (isTeamLead: boolean) => set({ isTeamLead }),
  isCurrentWeekReporter: false,
  setIsCurrentWeekReporter: (isCurrentWeekReporter: boolean) => set({ isCurrentWeekReporter }),
}));

export default userStore;