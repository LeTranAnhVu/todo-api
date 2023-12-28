CREATE TABLE public.repeatable
(
    id         uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    type       int NOT NULL,
    started_at timestamptz  NOT NULL,
    ended_at timestamptz,
    todo_id  uuid NOT NULL,
    CONSTRAINT fk_todo FOREIGN KEY (todo_id) REFERENCES public.todo (id) ON DELETE CASCADE,
    UNIQUE (type, todo_id)
)