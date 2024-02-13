import { create } from 'zustand';
import { MemberTeams } from '../types/WeeklyStatus.types';

type UserState = {
  role: 'TeamLead' | 'CurrentWeekReporter' | 'Normal' | null;
  teamId: number;
  teamName: string | null;
  memberId: number;
  memberName: string | null;
  isAuthenticated: boolean;
  memberActiveTeams: MemberTeams | null;
  setRole: (role: 'TeamLead' | 'CurrentWeekReporter' | 'Normal' | null) => void;
  setTeamId: (teamId: number) => void;
  setTeamName: (teamName: string) => void;
  setMemberId: (memberId: number) => void;
  setMemberName: (memberName: string) => void;
  setIsAuthenticated: (isAuthenticated: boolean) => void;
  setMemberActiveTeams: (memberTeams: MemberTeams) => void;
}

const userStore = create<UserState>((set) => ({
  role: null,
  setRole: (role) => set({ role }),
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
  setMemberActiveTeams: (memberActiveTeams: MemberTeams) => set({ memberActiveTeams })
}));

export default userStore;