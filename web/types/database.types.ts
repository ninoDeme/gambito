export type Json =
  | string
  | number
  | boolean
  | null
  | { [key: string]: Json | undefined }
  | Json[]

export type Database = {
  graphql_public: {
    Tables: {
      [_ in never]: never
    }
    Views: {
      [_ in never]: never
    }
    Functions: {
      graphql: {
        Args: {
          operationName?: string
          query?: string
          variables?: Json
          extensions?: Json
        }
        Returns: Json
      }
    }
    Enums: {
      [_ in never]: never
    }
    CompositeTypes: {
      [_ in never]: never
    }
  }
  public: {
    Tables: {
      defeito: {
        Row: {
          id: number
          nome: string
          organizacao: number | null
        }
        Insert: {
          id?: never
          nome: string
          organizacao?: number | null
        }
        Update: {
          id?: never
          nome?: string
          organizacao?: number | null
        }
        Relationships: [
          {
            foreignKeyName: "defeito_organizacao_fkey"
            columns: ["organizacao"]
            isOneToOne: false
            referencedRelation: "organizacao"
            referencedColumns: ["id"]
          },
        ]
      }
      etapa: {
        Row: {
          id: number
          nome: string
          organizacao: number | null
        }
        Insert: {
          id?: never
          nome: string
          organizacao?: number | null
        }
        Update: {
          id?: never
          nome?: string
          organizacao?: number | null
        }
        Relationships: [
          {
            foreignKeyName: "etapa_organizacao_fkey"
            columns: ["organizacao"]
            isOneToOne: false
            referencedRelation: "organizacao"
            referencedColumns: ["id"]
          },
        ]
      }
      funcao: {
        Row: {
          id: number
          nome: string
          organizacao: number | null
        }
        Insert: {
          id?: never
          nome: string
          organizacao?: number | null
        }
        Update: {
          id?: never
          nome?: string
          organizacao?: number | null
        }
        Relationships: [
          {
            foreignKeyName: "funcao_organizacao_fkey"
            columns: ["organizacao"]
            isOneToOne: false
            referencedRelation: "organizacao"
            referencedColumns: ["id"]
          },
        ]
      }
      funcionario: {
        Row: {
          encarregado: number | null
          funcao: number | null
          id: number
          invativo: boolean
          nome: string
          organizacao: number | null
        }
        Insert: {
          encarregado?: number | null
          funcao?: number | null
          id?: never
          invativo?: boolean
          nome: string
          organizacao?: number | null
        }
        Update: {
          encarregado?: number | null
          funcao?: number | null
          id?: never
          invativo?: boolean
          nome?: string
          organizacao?: number | null
        }
        Relationships: [
          {
            foreignKeyName: "funcionario_encarregado_fkey"
            columns: ["encarregado"]
            isOneToOne: false
            referencedRelation: "funcionario"
            referencedColumns: ["id"]
          },
          {
            foreignKeyName: "funcionario_funcao_fkey"
            columns: ["funcao"]
            isOneToOne: false
            referencedRelation: "funcao"
            referencedColumns: ["id"]
          },
          {
            foreignKeyName: "funcionario_organizacao_fkey"
            columns: ["organizacao"]
            isOneToOne: false
            referencedRelation: "organizacao"
            referencedColumns: ["id"]
          },
        ]
      }
      linha_producao: {
        Row: {
          descricao: string | null
          id: number
          organizacao: number | null
        }
        Insert: {
          descricao?: string | null
          id?: never
          organizacao?: number | null
        }
        Update: {
          descricao?: string | null
          id?: never
          organizacao?: number | null
        }
        Relationships: [
          {
            foreignKeyName: "linha_producao_organizacao_fkey"
            columns: ["organizacao"]
            isOneToOne: false
            referencedRelation: "organizacao"
            referencedColumns: ["id"]
          },
        ]
      }
      linha_producao_dia: {
        Row: {
          data: string
          invativo: boolean
          linha_producao: number
        }
        Insert: {
          data: string
          invativo?: boolean
          linha_producao: number
        }
        Update: {
          data?: string
          invativo?: boolean
          linha_producao?: number
        }
        Relationships: [
          {
            foreignKeyName: "linha_producao_dia_linha_producao_fkey"
            columns: ["linha_producao"]
            isOneToOne: false
            referencedRelation: "linha_producao"
            referencedColumns: ["id"]
          },
        ]
      }
      linha_producao_hora: {
        Row: {
          data: string | null
          hora: string
          hora_fim: string | null
          hora_ini: string | null
          id: number
          linha_producao: number
          paralizacao: boolean
          pedido: number
          qtd_produzido: number | null
          tipo: Database["public"]["Enums"]["tipo_hora"] | null
        }
        Insert: {
          data?: string | null
          hora: string
          hora_fim?: string | null
          hora_ini?: string | null
          id?: never
          linha_producao: number
          paralizacao?: boolean
          pedido: number
          qtd_produzido?: number | null
          tipo?: Database["public"]["Enums"]["tipo_hora"] | null
        }
        Update: {
          data?: string | null
          hora?: string
          hora_fim?: string | null
          hora_ini?: string | null
          id?: never
          linha_producao?: number
          paralizacao?: boolean
          pedido?: number
          qtd_produzido?: number | null
          tipo?: Database["public"]["Enums"]["tipo_hora"] | null
        }
        Relationships: [
          {
            foreignKeyName: "linha_producao_hora_linha_producao_data_fkey"
            columns: ["linha_producao", "data"]
            isOneToOne: false
            referencedRelation: "linha_producao_dia"
            referencedColumns: ["linha_producao", "data"]
          },
          {
            foreignKeyName: "linha_producao_hora_pedido_fkey"
            columns: ["pedido"]
            isOneToOne: false
            referencedRelation: "pedido"
            referencedColumns: ["id"]
          },
        ]
      }
      linha_producao_hora_defeito: {
        Row: {
          defeito: number
          linha_producao_hora: number
          qtd_pecas: number
          retrabalhado: boolean
        }
        Insert: {
          defeito: number
          linha_producao_hora: number
          qtd_pecas: number
          retrabalhado: boolean
        }
        Update: {
          defeito?: number
          linha_producao_hora?: number
          qtd_pecas?: number
          retrabalhado?: boolean
        }
        Relationships: [
          {
            foreignKeyName: "linha_producao_hora_defeito_defeito_fkey"
            columns: ["defeito"]
            isOneToOne: false
            referencedRelation: "defeito"
            referencedColumns: ["id"]
          },
          {
            foreignKeyName: "linha_producao_hora_defeito_linha_producao_hora_fkey"
            columns: ["linha_producao_hora"]
            isOneToOne: false
            referencedRelation: "linha_producao_hora"
            referencedColumns: ["id"]
          },
        ]
      }
      linha_producao_hora_etapa: {
        Row: {
          etapa: number | null
          id: number
          linha_producao_hora: number
          ordem: number
          segundos: number
        }
        Insert: {
          etapa?: number | null
          id?: never
          linha_producao_hora: number
          ordem: number
          segundos: number
        }
        Update: {
          etapa?: number | null
          id?: never
          linha_producao_hora?: number
          ordem?: number
          segundos?: number
        }
        Relationships: [
          {
            foreignKeyName: "linha_producao_hora_etapa_etapa_fkey"
            columns: ["etapa"]
            isOneToOne: false
            referencedRelation: "etapa"
            referencedColumns: ["id"]
          },
          {
            foreignKeyName: "linha_producao_hora_etapa_linha_producao_hora_fkey"
            columns: ["linha_producao_hora"]
            isOneToOne: false
            referencedRelation: "linha_producao_hora"
            referencedColumns: ["id"]
          },
        ]
      }
      linha_producao_hora_etapa_funcionario: {
        Row: {
          funcionario: number
          linha_producao_hora_etapa: number
        }
        Insert: {
          funcionario: number
          linha_producao_hora_etapa: number
        }
        Update: {
          funcionario?: number
          linha_producao_hora_etapa?: number
        }
        Relationships: [
          {
            foreignKeyName: "linha_producao_hora_etapa_funcio_linha_producao_hora_etapa_fkey"
            columns: ["linha_producao_hora_etapa"]
            isOneToOne: false
            referencedRelation: "linha_producao_hora_etapa"
            referencedColumns: ["id"]
          },
          {
            foreignKeyName: "linha_producao_hora_etapa_funcionario_funcionario_fkey"
            columns: ["funcionario"]
            isOneToOne: false
            referencedRelation: "funcionario"
            referencedColumns: ["id"]
          },
        ]
      }
      organizacao: {
        Row: {
          id: number
          nome: string
        }
        Insert: {
          id?: never
          nome: string
        }
        Update: {
          id?: never
          nome?: string
        }
        Relationships: []
      }
      pedido: {
        Row: {
          id: number
          produto: number
          qtd_pecas: number
        }
        Insert: {
          id?: never
          produto: number
          qtd_pecas: number
        }
        Update: {
          id?: never
          produto?: number
          qtd_pecas?: number
        }
        Relationships: [
          {
            foreignKeyName: "pedido_produto_fkey"
            columns: ["produto"]
            isOneToOne: false
            referencedRelation: "produto"
            referencedColumns: ["id"]
          },
        ]
      }
      produto: {
        Row: {
          id: number
          nome: string
          organizacao: number | null
          tempo_peca: number
        }
        Insert: {
          id?: never
          nome: string
          organizacao?: number | null
          tempo_peca: number
        }
        Update: {
          id?: never
          nome?: string
          organizacao?: number | null
          tempo_peca?: number
        }
        Relationships: [
          {
            foreignKeyName: "produto_organizacao_fkey"
            columns: ["organizacao"]
            isOneToOne: false
            referencedRelation: "organizacao"
            referencedColumns: ["id"]
          },
        ]
      }
      user_organizacao: {
        Row: {
          organizacao: number
          usuario: string
        }
        Insert: {
          organizacao: number
          usuario: string
        }
        Update: {
          organizacao?: number
          usuario?: string
        }
        Relationships: [
          {
            foreignKeyName: "user_organizacao_organizacao_fkey"
            columns: ["organizacao"]
            isOneToOne: false
            referencedRelation: "organizacao"
            referencedColumns: ["id"]
          },
        ]
      }
    }
    Views: {
      [_ in never]: never
    }
    Functions: {
      [_ in never]: never
    }
    Enums: {
      tipo_hora: "HORA_EXTRA" | "BANCO_HORAS"
    }
    CompositeTypes: {
      [_ in never]: never
    }
  }
}

type DefaultSchema = Database[Extract<keyof Database, "public">]

export type Tables<
  DefaultSchemaTableNameOrOptions extends
    | keyof (DefaultSchema["Tables"] & DefaultSchema["Views"])
    | { schema: keyof Database },
  TableName extends DefaultSchemaTableNameOrOptions extends {
    schema: keyof Database
  }
    ? keyof (Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"] &
        Database[DefaultSchemaTableNameOrOptions["schema"]]["Views"])
    : never = never,
> = DefaultSchemaTableNameOrOptions extends { schema: keyof Database }
  ? (Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"] &
      Database[DefaultSchemaTableNameOrOptions["schema"]]["Views"])[TableName] extends {
      Row: infer R
    }
    ? R
    : never
  : DefaultSchemaTableNameOrOptions extends keyof (DefaultSchema["Tables"] &
        DefaultSchema["Views"])
    ? (DefaultSchema["Tables"] &
        DefaultSchema["Views"])[DefaultSchemaTableNameOrOptions] extends {
        Row: infer R
      }
      ? R
      : never
    : never

export type TablesInsert<
  DefaultSchemaTableNameOrOptions extends
    | keyof DefaultSchema["Tables"]
    | { schema: keyof Database },
  TableName extends DefaultSchemaTableNameOrOptions extends {
    schema: keyof Database
  }
    ? keyof Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"]
    : never = never,
> = DefaultSchemaTableNameOrOptions extends { schema: keyof Database }
  ? Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"][TableName] extends {
      Insert: infer I
    }
    ? I
    : never
  : DefaultSchemaTableNameOrOptions extends keyof DefaultSchema["Tables"]
    ? DefaultSchema["Tables"][DefaultSchemaTableNameOrOptions] extends {
        Insert: infer I
      }
      ? I
      : never
    : never

export type TablesUpdate<
  DefaultSchemaTableNameOrOptions extends
    | keyof DefaultSchema["Tables"]
    | { schema: keyof Database },
  TableName extends DefaultSchemaTableNameOrOptions extends {
    schema: keyof Database
  }
    ? keyof Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"]
    : never = never,
> = DefaultSchemaTableNameOrOptions extends { schema: keyof Database }
  ? Database[DefaultSchemaTableNameOrOptions["schema"]]["Tables"][TableName] extends {
      Update: infer U
    }
    ? U
    : never
  : DefaultSchemaTableNameOrOptions extends keyof DefaultSchema["Tables"]
    ? DefaultSchema["Tables"][DefaultSchemaTableNameOrOptions] extends {
        Update: infer U
      }
      ? U
      : never
    : never

export type Enums<
  DefaultSchemaEnumNameOrOptions extends
    | keyof DefaultSchema["Enums"]
    | { schema: keyof Database },
  EnumName extends DefaultSchemaEnumNameOrOptions extends {
    schema: keyof Database
  }
    ? keyof Database[DefaultSchemaEnumNameOrOptions["schema"]]["Enums"]
    : never = never,
> = DefaultSchemaEnumNameOrOptions extends { schema: keyof Database }
  ? Database[DefaultSchemaEnumNameOrOptions["schema"]]["Enums"][EnumName]
  : DefaultSchemaEnumNameOrOptions extends keyof DefaultSchema["Enums"]
    ? DefaultSchema["Enums"][DefaultSchemaEnumNameOrOptions]
    : never

export type CompositeTypes<
  PublicCompositeTypeNameOrOptions extends
    | keyof DefaultSchema["CompositeTypes"]
    | { schema: keyof Database },
  CompositeTypeName extends PublicCompositeTypeNameOrOptions extends {
    schema: keyof Database
  }
    ? keyof Database[PublicCompositeTypeNameOrOptions["schema"]]["CompositeTypes"]
    : never = never,
> = PublicCompositeTypeNameOrOptions extends { schema: keyof Database }
  ? Database[PublicCompositeTypeNameOrOptions["schema"]]["CompositeTypes"][CompositeTypeName]
  : PublicCompositeTypeNameOrOptions extends keyof DefaultSchema["CompositeTypes"]
    ? DefaultSchema["CompositeTypes"][PublicCompositeTypeNameOrOptions]
    : never

export const Constants = {
  graphql_public: {
    Enums: {},
  },
  public: {
    Enums: {
      tipo_hora: ["HORA_EXTRA", "BANCO_HORAS"],
    },
  },
} as const

