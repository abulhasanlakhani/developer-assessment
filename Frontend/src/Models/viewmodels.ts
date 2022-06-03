export type fruit = {
  category: string
}

export type vegetable = {
  color: string
}

export type vegefruit = fruit | (vegetable & { size: number })
