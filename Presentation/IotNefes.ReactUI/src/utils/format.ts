export function formatNumber(value: number, digits = 1): string {
  return new Intl.NumberFormat('tr-TR', {
    minimumFractionDigits: digits,
    maximumFractionDigits: digits,
  }).format(value)
}

export function formatTime(iso: string): string {
  const d = new Date(iso)
  return d.toLocaleTimeString('tr-TR', {
    hour: '2-digit',
    minute: '2-digit',
  })
}

export function formatDateTime(iso: string): string {
  const d = new Date(iso)
  return d.toLocaleString('tr-TR', {
    day: '2-digit',
    month: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
  })
}

export function average(values: number[]): number {
  if (values.length === 0) return 0
  return values.reduce((a, b) => a + b, 0) / values.length
}

export function minMax(values: number[]): { min: number; max: number } {
  if (values.length === 0) return { min: 0, max: 0 }
  return {
    min: Math.min(...values),
    max: Math.max(...values),
  }
}

export function trend(values: number[]): 'up' | 'down' | 'flat' {
  if (values.length < 2) return 'flat'
  const first = values[0]
  const last = values[values.length - 1]
  const diff = last - first
  const threshold = Math.abs(first) * 0.02
  if (diff > threshold) return 'up'
  if (diff < -threshold) return 'down'
  return 'flat'
}

export function directionLabel(deg: number): string {
  const dirs = ['K', 'KD', 'D', 'GD', 'G', 'GB', 'B', 'KB']
  const idx = Math.round(deg / 45) % 8
  return dirs[idx]
}
