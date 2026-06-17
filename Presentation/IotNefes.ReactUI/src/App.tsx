import { useMemo } from 'react'
import {
  Activity,
  AlertTriangle,
  Cloud,
  Droplets,
  Gauge,
  RefreshCw,
  Thermometer,
  Wind,
  Sparkles,
} from 'lucide-react'
import './App.css'
import { useDashboardData } from './hooks/useDashboardData'
import { KpiCard } from './components/KpiCard'
import {
  MultiLineChart,
  type MultiLineSeries,
} from './components/MultiLineChart'
import { AirQualityGauge } from './components/AirQualityGauge'
import { WindCompass } from './components/WindCompass'
import { DeviceList } from './components/DeviceList'
import { SensorTable } from './components/SensorTable'
import {
  SENSOR_META,
  type ValueSensorResponse,
  type WindResponse,
  type AirTemperatureResponse,
} from './api/types'
import { average, formatTime, minMax } from './utils/format'

function latestOf<T extends { createdAt: string }>(items: T[]): T | null {
  if (items.length === 0) return null
  return [...items].sort(
    (a, b) =>
      new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime(),
  )[0]
}

function toSparkSeries(items: ValueSensorResponse[]) {
  return [...items]
    .sort(
      (a, b) =>
        new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime(),
    )
    .map((i) => ({ time: formatTime(i.createdAt), value: i.value }))
}

export default function App() {
  const { data, loading, error, lastUpdated, refresh } =
    useDashboardData(60_000)

  const combined = useMemo(() => {
    if (!data) return null

    const byTime = new Map<string, Record<string, number | string>>()

    function push<T extends { createdAt: string }>(
      items: T[],
      key: string,
      valueFn: (i: T) => number,
    ) {
      const sorted = [...items].sort(
        (a, b) =>
          new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime(),
      )
      for (const it of sorted) {
        const t = formatTime(it.createdAt)
        const row = byTime.get(t) ?? { time: t }
        row[key] = valueFn(it)
        byTime.set(t, row)
      }
    }

    push<ValueSensorResponse>(data.pm25.items, 'pm25', (i) => i.value)
    push<ValueSensorResponse>(data.pm10.items, 'pm10', (i) => i.value)
    push<ValueSensorResponse>(data.co2.items, 'co2', (i) => i.value)
    push<ValueSensorResponse>(
      data.temperature.items,
      'temperature',
      (i) => i.value,
    )
    push<ValueSensorResponse>(data.humidity.items, 'humidity', (i) => i.value)
    push<AirTemperatureResponse>(
      data.airTemperature.items,
      'airTemperature',
      (i) => i.temperature,
    )
    push<WindResponse>(data.wind.items, 'wind', (i) => i.speed)

    return Array.from(byTime.values())
  }, [data])

  if (loading && !data) {
    return (
      <div className="app">
        <div className="loading-screen">
          <Sparkles className="spin" size={36} />
          <div>Sensör verileri yükleniyor…</div>
        </div>
      </div>
    )
  }

  if (error) {
    return (
      <div className="app">
        <div className="error-screen">
          <AlertTriangle size={36} />
          <div>Veri çekilemedi: {error}</div>
          <button className="primary-btn" onClick={refresh}>
            <RefreshCw size={16} /> Tekrar dene
          </button>
        </div>
      </div>
    )
  }

  if (!data) return null

  const latestPm25 = latestOf(data.pm25.items)!
  const latestPm10 = latestOf(data.pm10.items)!
  const latestCo2 = latestOf(data.co2.items)!
  const latestTemp = latestOf(data.temperature.items)!
  const latestHum = latestOf(data.humidity.items)!
  const latestWind = latestOf(data.wind.items)!
  const latestAir = latestOf(data.airTemperature.items)!

  const pmSeries = toSparkSeries(data.pm25.items)
  const pm10Series = toSparkSeries(data.pm10.items)
  const tempSeries = toSparkSeries(data.temperature.items)
  const humSeries = toSparkSeries(data.humidity.items)
  const co2Series = toSparkSeries(data.co2.items)
  const airSeries = [...data.airTemperature.items]
    .sort(
      (a, b) =>
        new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime(),
    )
    .map((i) => ({ time: formatTime(i.createdAt), value: i.temperature }))
  const windSeries = [...data.wind.items]
    .sort(
      (a, b) =>
        new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime(),
    )
    .map((i) => ({ time: formatTime(i.createdAt), value: i.speed }))

  const mmPm25 = minMax(data.pm25.items.map((i) => i.value))
  const mmPm10 = minMax(data.pm10.items.map((i) => i.value))
  const mmTemp = minMax(data.temperature.items.map((i) => i.value))
  const mmHum = minMax(data.humidity.items.map((i) => i.value))
  const mmCo2 = minMax(data.co2.items.map((i) => i.value))
  const mmWind = minMax(data.wind.items.map((i) => i.speed))
  const mmAir = minMax(data.airTemperature.items.map((i) => i.temperature))

  const totalRecords =
    data.pm25.totalCount +
    data.pm10.totalCount +
    data.temperature.totalCount +
    data.humidity.totalCount +
    data.co2.totalCount +
    data.wind.totalCount +
    data.airTemperature.totalCount

  const airQualitySeries: MultiLineSeries[] = [
    {
      key: 'pm25',
      label: 'PM2.5',
      color: SENSOR_META.pm25.accent,
      unit: 'µg/m³',
    },
    {
      key: 'pm10',
      label: 'PM10',
      color: SENSOR_META.pm10.accent,
      unit: 'µg/m³',
    },
    { key: 'co2', label: 'CO₂', color: SENSOR_META.co2.accent, unit: 'ppm' },
  ]

  const climateSeries: MultiLineSeries[] = [
    {
      key: 'temperature',
      label: 'Sensör Sıcaklık',
      color: SENSOR_META.temperature.accent,
      unit: '°C',
    },
    {
      key: 'airTemperature',
      label: 'Hava Sıcaklık',
      color: SENSOR_META.airTemperature.accent,
      unit: '°C',
    },
    {
      key: 'humidity',
      label: 'Nem',
      color: SENSOR_META.humidity.accent,
      unit: '%',
    },
    {
      key: 'wind',
      label: 'Rüzgar',
      color: SENSOR_META.wind.accent,
      unit: 'm/s',
    },
  ]

  return (
    <div className="app">
      <header className="topbar">
        <div className="brand">
          <div className="brand-logo">
            <Activity size={20} />
          </div>
          <div>
            <div className="brand-title">IotNefes Dashboard</div>
            <div className="brand-sub">
              ESP32 sensör ağı · gerçek zamanlı izleme
            </div>
          </div>
        </div>
        <div className="topbar-meta">
          <div className="status-dot live" />
          <div className="meta-stack">
            <span>Son güncelleme</span>
            <strong>
              {lastUpdated
                ? lastUpdated.toLocaleTimeString('tr-TR')
                : '—'}
            </strong>
          </div>
          <button
            className="icon-btn"
            onClick={refresh}
            disabled={loading}
            title="Yenile"
          >
            <RefreshCw size={16} className={loading ? 'spin' : ''} />
          </button>
        </div>
      </header>

      <section className="summary-strip">
        <div className="summary-item">
          <span>Toplam ölçüm</span>
          <strong>{totalRecords}</strong>
        </div>
        <div className="summary-item">
          <span>Aktif sensör tipi</span>
          <strong>{Object.keys(SENSOR_META).length}</strong>
        </div>
        <div className="summary-item">
          <span>Ortalama PM2.5</span>
          <strong>
            {average(data.pm25.items.map((i) => i.value)).toFixed(1)} µg/m³
          </strong>
        </div>
        <div className="summary-item">
          <span>Ortalama Sıcaklık</span>
          <strong>
            {average(data.temperature.items.map((i) => i.value)).toFixed(1)} °C
          </strong>
        </div>
        <div className="summary-item">
          <span>Ortalama CO₂</span>
          <strong>
            {average(data.co2.items.map((i) => i.value)).toFixed(0)} ppm
          </strong>
        </div>
      </section>

      <section className="kpi-grid">
        <KpiCard
          meta={SENSOR_META.pm25}
          icon={<Cloud size={18} />}
          current={latestPm25.value}
          average={average(data.pm25.items.map((i) => i.value))}
          min={mmPm25.min}
          max={mmPm25.max}
          series={pmSeries}
        />
        <KpiCard
          meta={SENSOR_META.pm10}
          icon={<Cloud size={18} />}
          current={latestPm10.value}
          average={average(data.pm10.items.map((i) => i.value))}
          min={mmPm10.min}
          max={mmPm10.max}
          series={pm10Series}
        />
        <KpiCard
          meta={SENSOR_META.co2}
          icon={<Gauge size={18} />}
          current={latestCo2.value}
          average={average(data.co2.items.map((i) => i.value))}
          min={mmCo2.min}
          max={mmCo2.max}
          series={co2Series}
        />
        <KpiCard
          meta={SENSOR_META.temperature}
          icon={<Thermometer size={18} />}
          current={latestTemp.value}
          average={average(data.temperature.items.map((i) => i.value))}
          min={mmTemp.min}
          max={mmTemp.max}
          series={tempSeries}
        />
        <KpiCard
          meta={SENSOR_META.humidity}
          icon={<Droplets size={18} />}
          current={latestHum.value}
          average={average(data.humidity.items.map((i) => i.value))}
          min={mmHum.min}
          max={mmHum.max}
          series={humSeries}
        />
        <KpiCard
          meta={SENSOR_META.airTemperature}
          icon={<Thermometer size={18} />}
          current={latestAir.temperature}
          average={average(
            data.airTemperature.items.map((i) => i.temperature),
          )}
          min={mmAir.min}
          max={mmAir.max}
          series={airSeries}
        />
        <KpiCard
          meta={SENSOR_META.wind}
          icon={<Wind size={18} />}
          current={latestWind.speed}
          average={average(data.wind.items.map((i) => i.speed))}
          min={mmWind.min}
          max={mmWind.max}
          series={windSeries}
        />
      </section>

      <section className="chart-grid">
        <div className="panel wide">
          <div className="panel-header">
            <div>
              <h3>Hava Kalitesi Zaman Serisi</h3>
              <p>PM2.5, PM10 ve CO₂ — son 12 ölçüm</p>
            </div>
          </div>
          <MultiLineChart data={combined ?? []} series={airQualitySeries} />
        </div>
        <div className="panel">
          <div className="panel-header">
            <div>
              <h3>Hava Kalitesi İndeksi</h3>
              <p>Anlık ortalama değerlere göre</p>
            </div>
          </div>
          <AirQualityGauge
            pm25={latestPm25.value}
            pm10={latestPm10.value}
            co2={latestCo2.value}
          />
        </div>
      </section>

      <section className="chart-grid">
        <div className="panel wide">
          <div className="panel-header">
            <div>
              <h3>İklim ve Rüzgar</h3>
              <p>Sıcaklık, nem ve rüzgar hızı birlikte</p>
            </div>
          </div>
          <MultiLineChart data={combined ?? []} series={climateSeries} />
        </div>
        <div className="panel">
          <div className="panel-header">
            <div>
              <h3>Rüzgar Pusulası</h3>
              <p>Anlık yön ve hız</p>
            </div>
          </div>
          <WindCompass latest={latestWind} history={data.wind.items} />
        </div>
      </section>

      <section className="bottom-grid">
        <div className="panel">
          <div className="panel-header">
            <div>
              <h3>Cihazlar</h3>
              <p>Aktif ESP32 istasyonları</p>
            </div>
          </div>
          <DeviceList data={data} />
        </div>
        <div className="panel wide">
          <div className="panel-header">
            <div>
              <h3>Son Ölçümler</h3>
              <p>Tüm sensörlerden son kayıtlar</p>
            </div>
          </div>
          <SensorTable data={data} />
        </div>
      </section>

      <footer className="footer">
        IotNefes · Clean Architecture · ASP.NET Core 8 + React 19
      </footer>
    </div>
  )
}
