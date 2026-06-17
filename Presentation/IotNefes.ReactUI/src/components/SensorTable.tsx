import { useMemo, useState } from 'react'
import type { AllSensorData } from '../api/client'
import { SENSOR_META, type SensorKey } from '../api/types'
import { formatDateTime, formatNumber } from '../utils/format'

type Row = {
  id: string
  sensor: SensorKey
  deviceId: string
  primary: number
  secondary?: number
  createdAt: string
}

function buildRows(data: AllSensorData): Row[] {
  const rows: Row[] = []

  for (const it of data.pm25.items) {
    rows.push({
      id: it.id,
      sensor: 'pm25',
      deviceId: it.deviceId,
      primary: it.value,
      createdAt: it.createdAt,
    })
  }
  for (const it of data.pm10.items) {
    rows.push({
      id: it.id,
      sensor: 'pm10',
      deviceId: it.deviceId,
      primary: it.value,
      createdAt: it.createdAt,
    })
  }
  for (const it of data.temperature.items) {
    rows.push({
      id: it.id,
      sensor: 'temperature',
      deviceId: it.deviceId,
      primary: it.value,
      createdAt: it.createdAt,
    })
  }
  for (const it of data.humidity.items) {
    rows.push({
      id: it.id,
      sensor: 'humidity',
      deviceId: it.deviceId,
      primary: it.value,
      createdAt: it.createdAt,
    })
  }
  for (const it of data.co2.items) {
    rows.push({
      id: it.id,
      sensor: 'co2',
      deviceId: it.deviceId,
      primary: it.value,
      createdAt: it.createdAt,
    })
  }
  for (const it of data.wind.items) {
    rows.push({
      id: it.id,
      sensor: 'wind',
      deviceId: it.deviceId,
      primary: it.speed,
      secondary: it.direction,
      createdAt: it.createdAt,
    })
  }
  for (const it of data.airTemperature.items) {
    rows.push({
      id: it.id,
      sensor: 'airTemperature',
      deviceId: it.deviceId,
      primary: it.temperature,
      secondary: it.humidity ?? undefined,
      createdAt: it.createdAt,
    })
  }

  return rows.sort(
    (a, b) =>
      new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime(),
  )
}

export function SensorTable({ data }: { data: AllSensorData }) {
  const [filter, setFilter] = useState<SensorKey | 'all'>('all')
  const [query, setQuery] = useState('')

  const rows = useMemo(() => buildRows(data), [data])
  const filtered = useMemo(() => {
    return rows.filter((r) => {
      if (filter !== 'all' && r.sensor !== filter) return false
      if (query && !r.deviceId.toLowerCase().includes(query.toLowerCase()))
        return false
      return true
    })
  }, [rows, filter, query])

  return (
    <div className="table-wrap">
      <div className="table-toolbar">
        <div className="chip-group">
          <button
            className={`chip ${filter === 'all' ? 'active' : ''}`}
            onClick={() => setFilter('all')}
          >
            Tümü
            <span className="chip-count">{rows.length}</span>
          </button>
          {(Object.keys(SENSOR_META) as SensorKey[]).map((k) => {
            const meta = SENSOR_META[k]
            const count = rows.filter((r) => r.sensor === k).length
            return (
              <button
                key={k}
                className={`chip ${filter === k ? 'active' : ''}`}
                onClick={() => setFilter(k)}
                style={{ ['--chip-accent' as string]: meta.accent }}
              >
                {meta.label}
                <span className="chip-count">{count}</span>
              </button>
            )
          })}
        </div>
        <input
          className="search-input"
          placeholder="Cihaz ara (örn. ESP32-A1)"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
        />
      </div>

      <div className="table-scroll">
        <table className="sensor-table">
          <thead>
            <tr>
              <th>Sensör</th>
              <th>Cihaz</th>
              <th className="num">Değer</th>
              <th className="num">Ek</th>
              <th>Zaman</th>
            </tr>
          </thead>
          <tbody>
            {filtered.slice(0, 80).map((r) => {
              const meta = SENSOR_META[r.sensor]
              return (
                <tr key={r.id}>
                  <td>
                    <span
                      className="pill"
                      style={{
                        ['--pill-accent' as string]: meta.accent,
                      }}
                    >
                      <i />
                      {meta.label}
                    </span>
                  </td>
                  <td className="mono">{r.deviceId}</td>
                  <td className="num">
                    <strong>{formatNumber(r.primary, 1)}</strong>{' '}
                    <span className="muted">{meta.unit}</span>
                  </td>
                  <td className="num muted">
                    {r.secondary != null
                      ? r.sensor === 'wind'
                        ? `${formatNumber(r.secondary, 0)}°`
                        : `${formatNumber(r.secondary, 1)} %`
                      : '—'}
                  </td>
                  <td className="muted">{formatDateTime(r.createdAt)}</td>
                </tr>
              )
            })}
            {filtered.length === 0 && (
              <tr>
                <td colSpan={5} className="empty">
                  Kayıt bulunamadı.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  )
}
