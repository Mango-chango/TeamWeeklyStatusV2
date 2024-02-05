import { create } from 'zustand';

type UserState = {
  role: 'TeamLead' | 'CurrentWeekReporter' | 'Normal' | null;
  teamId: number;
  teamName: string | null;
  memberId: number;
  memberName: string | null;
  isAuthenticated: boolean;
  setRole: (role: 'TeamLead' | 'CurrentWeekReporter' | 'Normal' | null) => void;
  setTeamName: (teamName: string) => void;
  setMemberId: (memberId: number) => void;
  setMemberName: (memberName: string) => void;
  setIsAuthenticated: (isAuthenticated: boolean) => void;
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
}));

export default userStore;