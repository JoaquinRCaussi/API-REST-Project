export interface FormField {
    type: string;
    name: string;
    label: string;
    required?: boolean;
    options?: { label: string, value: any }[];
  }
  