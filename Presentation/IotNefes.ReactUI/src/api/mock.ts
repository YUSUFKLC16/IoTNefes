import type {
  AirTemperatureResponse,
  Co2Response,
  HumidityResponse,
  PagedResult,
  Pm10Response,
  Pm25Response,
  TemperatureResponse,
  ValueSensorResponse,
  WindResponse,
} from './types'

const DEVICES = ['ESP32-A1', 'ESP32-B2', 'ESP32-C3', 'ESP32-D4']

// İstanbul merkezi koordinatları etrafında sahte konumlar
const BASE_LAT = 41.015137
const BASE_LON = 28.97953

function rand(min: number, max: number): number {
  return Math.random() * (max - min) + min
}

function pseudoId(prefix: string, i: number): string {
  return `${prefix}-${Date.now().toString(36)}-${i.toString().padStart(3, '0')}`
}

function buildTimestamps(count: number): string[] {
  const now = Date.now()
  const stepMs = 15 * 60 * 1000 // 15 dk aralık
  return Array.from({ length: count }, (_, i) =>
    new Date(now - (count - 1 - i) * stepMs).toISOString(),
  )
}

function pickDevice(i: number): string {
  return DEVICES[i % DEVICES.length]
}

function jitterCoords(): { latitude: number; longitude: number } {
  return {
    latitude: BASE_LAT + rand(-0.05, 0.05),
    longitude: BASE_LON + rand(-0.05, 0.05),
  }
}

function generateValueSeries(
  prefix: string,
  count: number,
  min: number,
  max: number,
  noise: number,
): ValueSensorResponse[] {
  const timestamps = buildTimestamps(count)
  let value = rand(min, max)
  return timestamps.map((createdAt, i) => {
    value = Math.max(
      min,
      Math.min(max, value + rand(-noise, noise)),
    )
    const coords = jitterCoords()
    return {
      id: pseudoId(prefix, i),
      deviceId: pickDevice(i),
      value: Number(value.toFixed(2)),
      latitude: coords.latitude,
      longitude: coords.longitude,
      createdAt,
      updatedAt: null,
    }
  })
}

function toPaged<T>(items: T[]): PagedResult<T> {
  return {
    items,
    page: 1,
    pageSize: items.length <= 10 ? 10 : items.length <= 50 ? 50 : 100,
    totalCount: items.length,
    totalPages: 1,
    hasPrevious: false,
    hasNext: false,
  }
}

export function mockPm25(count = 12): PagedResult<Pm25Response> {
  return toPaged(generateValueSeries('pm25', count, 8, 65, 6))
}

export function mockPm10(count = 12): PagedResult<Pm10Response> {
  return toPaged(generateValueSeries('pm10', count, 15, 95, 8))
}

export function mockTemperature(count = 12): PagedResult<TemperatureResponse> {
  return toPaged(generateValueSeries('temp', count, 18, 32, 1.2))
}

export function mockHumidity(count = 12): PagedResult<HumidityResponse> {
  return toPaged(generateValueSeries('hum', count, 35, 78, 3))
}

export function mockCo2(count = 12): PagedResult<Co2Response> {
  return toPaged(generateValueSeries('co2', count, 380, 1200, 40))
}

export function mockWind(count = 12): PagedResult<WindResponse> {
  const timestamps = buildTimestamps(count)
  let speed = rand(1, 8)
  let direction = rand(0, 360)
  const items: WindResponse[] = timestamps.map((createdAt, i) => {
    speed = Math.max(0, Math.min(18, speed + rand(-1.2, 1.2)))
    direction = (direction + rand(-25, 25) + 360) % 360
    const coords = jitterCoords()
    return {
      id: pseudoId('wind', i),
      deviceId: pickDevice(i),
      speed: Number(speed.toFixed(2)),
      direction: Number(direction.toFixed(1)),
      latitude: coords.latitude,
      longitude: coords.longitude,
      createdAt,
      updatedAt: null,
    }
  })
  return toPaged(items)
}

export function mockAirTemperature(
  count = 12,
): PagedResult<AirTemperatureResponse> {
  const timestamps = buildTimestamps(count)
  let temperature = rand(16, 28)
  let humidity = rand(40, 70)
  const items: AirTemperatureResponse[] = timestamps.map((createdAt, i) => {
    temperature = Math.max(
      -5,
      Math.min(40, temperature + rand(-0.8, 0.8)),
    )
    humidity = Math.max(20, Math.min(95, humidity + rand(-2.5, 2.5)))
    const coords = jitterCoords()
    return {
      id: pseudoId('air', i),
      deviceId: pickDevice(i),
      temperature: Number(temperature.toFixed(2)),
      humidity: Number(humidity.toFixed(1)),
      latitude: coords.latitude,
      longitude: coords.longitude,
      createdAt,
      updatedAt: null,
    }
  })
  return toPaged(items)
}
