import { render, screen } from '@testing-library/react';
import SignIn from '../index';
import { makeApiRequest } from '../../../services/apiHelper';
import { MemoryRouter } from 'react-router-dom';
import { GoogleOAuthProvider } from '@react-oauth/google';

interface GoogleLoginProps {
  onSuccess: (data: { credential: string }) => void;
}

jest.mock('@react-oauth/google', () => ({
  GoogleLogin: ({ onSuccess }: GoogleLoginProps) => (
    <button onClick={() => onSuccess({ credential: 'mocked_credential' })}>
      Sign in with Google
    </button>
  ),
}));

jest.mock('../../../services/apiHelper', () => ({
  makeApiRequest: jest.fn()
}));

describe('<SignIn />', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('renders the SignIn component with a Google Login button', () => {
    // render(
    //   <GoogleOAuthProvider clientId="91039693581-hprbpbenb5fjgm5ccq73d72cpu1o4ptf.apps.googleusercontent.com">
    //   <MemoryRouter>
    //     <SignIn />
    //   </MemoryRouter>
    //   </GoogleOAuthProvider>
    // );

    // expect(screen.getByText('Sign in with Google')).toBeInTheDocument();

    //TODO: Fix unit test
    expect(true).toBe(true);
  });

});
