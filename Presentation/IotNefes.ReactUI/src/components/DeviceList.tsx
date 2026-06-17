import { Cpu } from 'lucide-react'
import type { AllSensorData } from '../api/client'
import { formatTime } from '../utils/format'

interface DeviceSummary {
  deviceId: string
  sensorCount: number
  lastSeen: string
  latitude?: number | null
  longitude?: number | null
}

export function DeviceList({ data }: { data: AllSensorData }) {
  const map = new Map<string, DeviceSummary>()
  const now = new Date()

  const allItems = [
    ...data.pm25.items,
    ...data.pm10.items,
    ...data.temperature.items,
    ...data.humidity.items,
    ...data.co2.items,
    ...data.wind.items,
    ...data.airTemperature.items,
  ]

  for (const it of allItems) {
    const existing = map.get(it.deviceId)
    if (!existing) {
      map.set(it.deviceId, {
        deviceId: it.deviceId,
        sensorCount: 1,
        lastSeen: it.createdAt,
        latitude: it.latitude,
        longitude: it.longitude,
      })
    } else {
      existing.sensorCount += 1
      if (new Date(it.createdAt) > new Date(existing.lastSeen)) {
        existing.lastSeen = it.createdAt
        existing.latitude = it.latitude
        existing.longitude = it.longitude
      }
    }
  }

  const devices = Array.from(map.values()).sort((a, b) =>
    a.deviceId.localeCompare(b.deviceId),
  )

  return (
    <div className="device-list">
      {devices.map((d) => {
        const ageMin =
          (now.getTime() - new Date(d.lastSeen).getTime()) / (60 * 1000)
        const online = ageMin < 30
        return (
          <div key={d.deviceId} className="device-row">
            <div className="device-icon">
              <Cpu size={18} />
            </div>
            <div className="device-main">
              <div className="device-id">{d.deviceId}</div>
              <div className="device-meta">
                {d.sensorCount} ölçüm · son: {formatTime(d.lastSeen)}
              </div>
            </div>
            {d.latitude != null && d.longitude != null && (
              <div className="device-coords">
                {d.latitude.toFixed(3)}, {d.longitude.toFixed(3)}
              </div>
            )}
            <span className={`device-status ${online ? 'online' : 'offline'}`}>
              <i />
              {online ? 'Çevrimiçi' : 'Uykuda'}
            </span>
          </div>
        )
      })}
    </div>
  )
}
