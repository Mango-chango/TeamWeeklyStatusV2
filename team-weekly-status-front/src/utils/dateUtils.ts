import { parseISO, format } from 'date-fns';

export const formatDate = (dateString: string | null | undefined): string => {
  if (!dateString) {
    return '-';
  }

  try {
    const date = parseISO(dateString);
    return format(date, 'MMMM d, yyyy'); // Formats date as "September 30, 2024"
  } catch (error) {
    console.error('Date parsing error:', error);
    return '-';
  }
};
