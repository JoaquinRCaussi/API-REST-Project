export interface FormField {
    placeholder?: string;
    type: string;
    name: string;
    label: string;
    required?: boolean;
    options?: { label: string, value: any }[];
  }
  