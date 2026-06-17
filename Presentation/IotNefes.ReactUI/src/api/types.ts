export interface PagedResult<T> {
  items: T[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasPrevious: boolean
  hasNext: boolean
}

export interface BaseSensorResponse {
  id: string
  deviceId: string
  latitude?: number | null
  longitude?: number | null
  createdAt: string
  updatedAt?: string | null
}

export interface ValueSensorResponse extends BaseSensorResponse {
  value: number
}

export type Pm25Response = ValueSensorResponse
export type Pm10Response = ValueSensorResponse
export type TemperatureResponse = ValueSensorResponse
export type HumidityResponse = ValueSensorResponse
export type Co2Response = ValueSensorResponse

export interface WindResponse extends BaseSensorResponse {
  speed: number
  direction: number
}

export interface AirTemperatureResponse extends BaseSensorResponse {
  temperature: number
  humidity?: number | null
}

export type SensorKey =
  | 'pm25'
  | 'pm10'
  | 'temperature'
  | 'humidity'
  | 'co2'
  | 'wind'
  | 'airTemperature'

export interface SensorMeta {
  key: SensorKey
  label: string
  endpoint: string
  unit: string
  accent: string
  description: string
}

export const SENSOR_META: Record<SensorKey, SensorMeta> = {
  pm25: {
    key: 'pm25',
    label: 'PM2.5',
    endpoint: 'Pm25',
    unit: 'µg/m³',
    accent: '#f472b6',
    description: 'İnce partikül madde',
  },
  pm10: {
    key: 'pm10',
    label: 'PM10',
    endpoint: 'Pm10',
    unit: 'µg/m³',
    accent: '#fb923c',
    description: 'Kaba partikül madde',
  },
  temperature: {
    key: 'temperature',
    label: 'Sıcaklık',
    endpoint: 'Temperature',
    unit: '°C',
    accent: '#f87171',
    description: 'Sensör sıcaklığı',
  },
  humidity: {
    key: 'humidity',
    label: 'Nem',
    endpoint: 'Humidity',
    unit: '%',
    accent: '#38bdf8',
    description: 'Bağıl nem oranı',
  },
  co2: {
    key: 'co2',
    label: 'CO₂',
    endpoint: 'Co2',
    unit: 'ppm',
    accent: '#a78bfa',
    description: 'Karbondioksit seviyesi',
  },
  wind: {
    key: 'wind',
    label: 'Rüzgar',
    endpoint: 'Wind',
    unit: 'm/s',
    accent: '#34d399',
    description: 'Rüzgar hızı ve yönü',
  },
  airTemperature: {
    key: 'airTemperature',
    label: 'Hava Sıcaklığı',
    endpoint: 'AirTemperature',
    unit: '°C',
    accent: '#facc15',
    description: 'Dış hava sıcaklığı + nem',
  },
}
