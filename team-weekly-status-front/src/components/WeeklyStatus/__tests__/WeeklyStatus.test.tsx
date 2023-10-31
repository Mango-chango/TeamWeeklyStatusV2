import { render, fireEvent, waitFor, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import WeeklyStatus from '../index';
import { makeApiRequest } from '../../../services/apiHelper';

jest.mock('../../../services/apiHelper', () => ({
    makeApiRequest: jest.fn()
  }));
  

describe("<WeeklyStatus />", () => {
  test("renders the WeeklyStatus component", () => {
    render(
      <MemoryRouter>
        <WeeklyStatus />
      </MemoryRouter>
    );
    expect(screen.getByText(/Team/)).toBeInTheDocument();
    expect(screen.getByText(/Weekly Status:/)).toBeInTheDocument();
  });
});