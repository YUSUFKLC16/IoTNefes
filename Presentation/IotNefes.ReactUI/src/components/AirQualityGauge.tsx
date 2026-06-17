import {
  PolarAngleAxis,
  RadialBar,
  RadialBarChart,
  ResponsiveContainer,
} from 'recharts'
import { formatNumber } from '../utils/format'

interface AirQualityGaugeProps {
  pm25: number
  pm10: number
  co2: number
}

interface AqiLevel {
  label: string
  color: string
  score: number
}

function pm25Level(v: number): AqiLevel {
  if (v <= 12) return { label: 'İyi', color: '#34d399', score: 90 }
  if (v <= 35) return { label: 'Orta', color: '#facc15', score: 70 }
  if (v <= 55) return { label: 'Hassas', color: '#fb923c', score: 50 }
  if (v <= 150) return { label: 'Sağlıksız', color: '#ef4444', score: 30 }
  return { label: 'Tehlikeli', color: '#b91c1c', score: 15 }
}

function pm10Level(v: number): AqiLevel {
  if (v <= 54) return { label: 'İyi', color: '#34d399', score: 90 }
  if (v <= 154) return { label: 'Orta', color: '#facc15', score: 70 }
  if (v <= 254) return { label: 'Hassas', color: '#fb923c', score: 50 }
  return { label: 'Sağlıksız', color: '#ef4444', score: 30 }
}

function co2Level(v: number): AqiLevel {
  if (v <= 600) return { label: 'İyi', color: '#34d399', score: 90 }
  if (v <= 1000) return { label: 'Orta', color: '#facc15', score: 70 }
  if (v <= 1500) return { label: 'Kötü', color: '#fb923c', score: 45 }
  return { label: 'Tehlikeli', color: '#ef4444', score: 20 }
}

export function AirQualityGauge({ pm25, pm10, co2 }: AirQualityGaugeProps) {
  const a = pm25Level(pm25)
  const b = pm10Level(pm10)
  const c = co2Level(co2)
  const avgScore = Math.round((a.score + b.score + c.score) / 3)
  const overall: AqiLevel =
    avgScore >= 80
      ? { label: 'İyi', color: '#34d399', score: avgScore }
      : avgScore >= 60
        ? { label: 'Orta', color: '#facc15', score: avgScore }
        : avgScore >= 40
          ? { label: 'Hassas', color: '#fb923c', score: avgScore }
          : { label: 'Sağlıksız', color: '#ef4444', score: avgScore }

  const data = [{ name: 'AQI', value: avgScore, fill: overall.color }]

  return (
    <div className="aqi-wrap">
      <div className="aqi-gauge">
        <ResponsiveContainer width="100%" height={220}>
          <RadialBarChart
            innerRadius="75%"
            outerRadius="100%"
            data={data}
            startAngle={210}
            endAngle={-30}
          >
            <PolarAngleAxis
              type="number"
              domain={[0, 100]}
              angleAxisId={0}
              tick={false}
            />
            <RadialBar
              background={{ fill: 'rgba(255,255,255,0.06)' }}
              dataKey="value"
              cornerRadius={12}
            />
          </RadialBarChart>
        </ResponsiveContainer>
        <div className="aqi-center">
          <div className="aqi-score" style={{ color: overall.color }}>
            {avgScore}
          </div>
          <div className="aqi-label">{overall.label}</div>
          <div className="aqi-sub">Hava Kalitesi</div>
        </div>
      </div>

      <div className="aqi-breakdown">
        <div className="aqi-row">
          <span className="aqi-dot" style={{ background: a.color }} />
          <div className="aqi-info">
            <div className="aqi-name">PM2.5</div>
            <div className="aqi-meta">
              {formatNumber(pm25, 1)} µg/m³ · {a.label}
            </div>
          </div>
        </div>
        <div className="aqi-row">
          <span className="aqi-dot" style={{ background: b.color }} />
          <div className="aqi-info">
            <div className="aqi-name">PM10</div>
            <div className="aqi-meta">
              {formatNumber(pm10, 1)} µg/m³ · {b.label}
            </div>
          </div>
        </div>
        <div className="aqi-row">
          <span className="aqi-dot" style={{ background: c.color }} />
          <div className="aqi-info">
            <div className="aqi-name">CO₂</div>
            <div className="aqi-meta">
              {formatNumber(co2, 0)} ppm · {c.label}
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
