CREATE TABLE public.repeatable
(
    id         uuid PRIMARY KEY DEFAULT uuid_generate_v4(),
    type       VARCHAR(50) NOT NULL,
    started_at timestamptz  NOT NULL,
    ended_at timestamptz,
    todo_id  uuid NOT NULL,
    CONSTRAINT fk_todo FOREIGN KEY (todo_id) REFERENCES public.todo (id),
    UNIQUE (type, todo_id)
)