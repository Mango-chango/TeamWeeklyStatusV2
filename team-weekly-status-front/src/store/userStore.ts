import { create } from 'zustand';

type UserState = {
  role: 'TeamLead' | 'CurrentWeekReporter' | 'Normal' | null;
  setRole: (role: 'TeamLead' | 'CurrentWeekReporter' | 'Normal' | null) => void;
}

const userStore = create<UserState>((set) => ({
  role: null,
  setRole: (role) => set({ role }),
}));

export default userStore;