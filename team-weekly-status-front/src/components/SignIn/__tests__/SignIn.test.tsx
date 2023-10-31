import React from 'react';
import { render, fireEvent, waitFor, screen } from '@testing-library/react';
import SignIn from '../index';
import { makeApiRequest } from '../../../services/apiHelper';
import { MemoryRouter } from 'react-router-dom';

jest.mock('../../../services/apiHelper', () => ({
  makeApiRequest: jest.fn()
}));

describe('<SignIn />', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('renders the SignIn component', () => {
    render(
      <MemoryRouter>
        <SignIn />
      </MemoryRouter>
    );
    expect(screen.getByLabelText(/User/)).toBeInTheDocument();
  });

  test('should show error on invalid email', async () => {
    // Setup mock response for the makeApiRequest function
    (makeApiRequest as jest.MockedFunction<typeof makeApiRequest>).mockResolvedValueOnce({
      success: false
    });

    render(
      <MemoryRouter>
        <SignIn />
      </MemoryRouter>
    );
    
    fireEvent.change(screen.getByPlaceholderText('Email prefix'), {
      target: { value: 'unknownChango' },
    });

    fireEvent.click(screen.getByText('Sign In'));

    // Wait for the error message to appear
    await waitFor(() => screen.getByText('Invalid email address. Please check and try again.'));
    expect(screen.getByText('Invalid email address. Please check and try again.')).toBeInTheDocument();
  });

});
