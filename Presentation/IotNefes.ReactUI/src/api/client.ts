import type {
  AirTemperatureResponse,
  Co2Response,
  HumidityResponse,
  PagedResult,
  Pm10Response,
  Pm25Response,
  TemperatureResponse,
  WindResponse,
} from './types'
import {
  mockAirTemperature,
  mockCo2,
  mockHumidity,
  mockPm10,
  mockPm25,
  mockTemperature,
  mockWind,
} from './mock'

// Gerçek API için base URL. DB ayağa kalktığında aşağıdaki fetch
// bloklarındaki yorumu kaldırın, mock çağrılarını silin.
// export const API_BASE_URL = 'https://localhost:5001/api'

export interface PagedParams {
  page?: number
  pageSize?: 10 | 50 | 100
}

/*
// Gerçek backend hazır olunca kullanılacak ortak fetch helper'ı:
async function getPaged<T>(
  endpoint: string,
  { page = 1, pageSize = 10 }: PagedParams = {},
): Promise<PagedResult<T>> {
  const url = new URL(`${API_BASE_URL}/${endpoint}/paged`)
  url.searchParams.set('page', String(page))
  url.searchParams.set('pageSize', String(pageSize))

  const response = await fetch(url.toString(), {
    method: 'GET',
    headers: { Accept: 'application/json' },
  })

  if (!response.ok) {
    throw new Error(`API isteği başarısız (${response.status}) - ${endpoint}`)
  }

  return (await response.json()) as PagedResult<T>
}
*/

// Mock gecikmesi (ms) - gerçek API hissiyatı için
const MOCK_LATENCY = 350

function withLatency<T>(value: T): Promise<T> {
  return new Promise((resolve) => {
    setTimeout(() => resolve(value), MOCK_LATENCY)
  })
}

export function getPm25Paged(
  params?: PagedParams,
): Promise<PagedResult<Pm25Response>> {
  // return getPaged<Pm25Response>('Pm25', params)
  return withLatency(mockPm25(params?.pageSize ?? 12))
}

export function getPm10Paged(
  params?: PagedParams,
): Promise<PagedResult<Pm10Response>> {
  // return getPaged<Pm10Response>('Pm10', params)
  return withLatency(mockPm10(params?.pageSize ?? 12))
}

export function getTemperaturePaged(
  params?: PagedParams,
): Promise<PagedResult<TemperatureResponse>> {
  // return getPaged<TemperatureResponse>('Temperature', params)
  return withLatency(mockTemperature(params?.pageSize ?? 12))
}

export function getHumidityPaged(
  params?: PagedParams,
): Promise<PagedResult<HumidityResponse>> {
  // return getPaged<HumidityResponse>('Humidity', params)
  return withLatency(mockHumidity(params?.pageSize ?? 12))
}

export function getCo2Paged(
  params?: PagedParams,
): Promise<PagedResult<Co2Response>> {
  // return getPaged<Co2Response>('Co2', params)
  return withLatency(mockCo2(params?.pageSize ?? 12))
}

export function getWindPaged(
  params?: PagedParams,
): Promise<PagedResult<WindResponse>> {
  // return getPaged<WindResponse>('Wind', params)
  return withLatency(mockWind(params?.pageSize ?? 12))
}

export function getAirTemperaturePaged(
  params?: PagedParams,
): Promise<PagedResult<AirTemperatureResponse>> {
  // return getPaged<AirTemperatureResponse>('AirTemperature', params)
  return withLatency(mockAirTemperature(params?.pageSize ?? 12))
}

export interface AllSensorData {
  pm25: PagedResult<Pm25Response>
  pm10: PagedResult<Pm10Response>
  temperature: PagedResult<TemperatureResponse>
  humidity: PagedResult<HumidityResponse>
  co2: PagedResult<Co2Response>
  wind: PagedResult<WindResponse>
  airTemperature: PagedResult<AirTemperatureResponse>
}

export async function getAllSensorsPaged(
  params?: PagedParams,
): Promise<AllSensorData> {
  const [pm25, pm10, temperature, humidity, co2, wind, airTemperature] =
    await Promise.all([
      getPm25Paged(params),
      getPm10Paged(params),
      getTemperaturePaged(params),
      getHumidityPaged(params),
      getCo2Paged(params),
      getWindPaged(params),
      getAirTemperaturePaged(params),
    ])

  return { pm25, pm10, temperature, humidity, co2, wind, airTemperature }
}
