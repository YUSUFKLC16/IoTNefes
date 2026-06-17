import { directionLabel, formatNumber } from '../utils/format'
import type { WindResponse } from '../api/types'

interface WindCompassProps {
  latest: WindResponse
  history: WindResponse[]
}

export function WindCompass({ latest, history }: WindCompassProps) {
  const size = 200
  const center = size / 2
  const radius = 78
  const angleRad = ((latest.direction - 90) * Math.PI) / 180
  const x2 = center + radius * Math.cos(angleRad)
  const y2 = center + radius * Math.sin(angleRad)

  const avgSpeed =
    history.reduce((acc, h) => acc + h.speed, 0) / (history.length || 1)
  const maxSpeed = history.reduce((acc, h) => Math.max(acc, h.speed), 0)

  const cardinals = [
    { label: 'K', deg: 0 },
    { label: 'D', deg: 90 },
    { label: 'G', deg: 180 },
    { label: 'B', deg: 270 },
  ]

  return (
    <div className="wind-wrap">
      <svg
        viewBox={`0 0 ${size} ${size}`}
        width="100%"
        height={size}
        className="wind-svg"
      >
        <circle
          cx={center}
          cy={center}
          r={radius + 16}
          fill="rgba(52, 211, 153, 0.05)"
          stroke="rgba(255,255,255,0.08)"
          strokeWidth={1}
        />
        <circle
          cx={center}
          cy={center}
          r={radius}
          fill="transparent"
          stroke="rgba(255,255,255,0.1)"
          strokeWidth={1}
          strokeDasharray="3 4"
        />
        {cardinals.map((c) => {
          const r = ((c.deg - 90) * Math.PI) / 180
          const lx = center + (radius + 8) * Math.cos(r)
          const ly = center + (radius + 8) * Math.sin(r)
          return (
            <text
              key={c.label}
              x={lx}
              y={ly}
              textAnchor="middle"
              dominantBaseline="middle"
              fill="rgba(255,255,255,0.55)"
              fontSize="11"
              fontWeight={600}
            >
              {c.label}
            </text>
          )
        })}

        {history.slice(-8).map((h, i) => {
          const hr = ((h.direction - 90) * Math.PI) / 180
          const hx = center + radius * 0.6 * Math.cos(hr)
          const hy = center + radius * 0.6 * Math.sin(hr)
          const opacity = (i + 1) / 10
          return (
            <circle
              key={h.id}
              cx={hx}
              cy={hy}
              r={3}
              fill="#34d399"
              fillOpacity={opacity}
            />
          )
        })}

        <line
          x1={center}
          y1={center}
          x2={x2}
          y2={y2}
          stroke="#34d399"
          strokeWidth={3}
          strokeLinecap="round"
        />
        <circle cx={center} cy={center} r={5} fill="#34d399" />
      </svg>

      <div className="wind-info">
        <div className="wind-metric">
          <div className="wind-value">{formatNumber(latest.speed, 1)}</div>
          <div className="wind-unit">m/s</div>
        </div>
        <div className="wind-direction">
          <span>{directionLabel(latest.direction)}</span>
          <small>{formatNumber(latest.direction, 0)}°</small>
        </div>
        <div className="wind-stats">
          <div>
            <span>Ort</span>
            <strong>{formatNumber(avgSpeed, 1)} m/s</strong>
          </div>
          <div>
            <span>Max</span>
            <strong>{formatNumber(maxSpeed, 1)} m/s</strong>
          </div>
        </div>
      </div>
    </div>
  )
}
