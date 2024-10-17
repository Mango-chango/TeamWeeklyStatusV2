/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_SHOW_CONTENT_MODAL: string;
  // Add other environment variables here...
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
