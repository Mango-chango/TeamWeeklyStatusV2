import { create } from 'zustand';

type UserState = {
  role: 'TeamLead' | 'CurrentWeekReporter' | 'Normal' | null;
  teamName: string | null;
  memberId: number;
  memberName: string | null;
  setRole: (role: 'TeamLead' | 'CurrentWeekReporter' | 'Normal' | null) => void;
  setTeamName: (teamName: string) => void;
  setMemberId: (memberId: number) => void;
  setMemberName: (memberName: string) => void;
}

const userStore = create<UserState>((set) => ({
  role: null,
  setRole: (role) => set({ role }),
  teamName: null,
  setTeamName: (teamName) => set({ teamName }),
  memberId: 0,
  setMemberId: (memberId) => set({ memberId }),
  memberName: null,
  setMemberName: (memberName) => set({ memberName }),
}));

export default userStore;