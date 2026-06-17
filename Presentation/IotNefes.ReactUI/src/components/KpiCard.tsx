import { AreaChart, Area, ResponsiveContainer, Tooltip } from 'recharts'
import { ArrowDownRight, ArrowUpRight, Minus } from 'lucide-react'
import { formatNumber, trend } from '../utils/format'
import type { SensorMeta } from '../api/types'

export interface KpiCardProps {
  meta: SensorMeta
  icon: React.ReactNode
  current: number
  average: number
  min: number
  max: number
  series: { time: string; value: number }[]
}

interface SparkTooltipProps {
  active?: boolean
  label?: string | number
  payload?: Array<{ value?: number | string }>
}

function CustomTooltip({ active, payload, label }: SparkTooltipProps) {
  if (!active || !payload || payload.length === 0) return null
  const raw = payload[0]?.value
  const value = typeof raw === 'number' ? raw : Number(raw)
  if (Number.isNaN(value)) return null
  return (
    <div className="chart-tooltip">
      <div className="chart-tooltip-time">{label}</div>
      <div className="chart-tooltip-value">{formatNumber(value, 1)}</div>
    </div>
  )
}

export function KpiCard({
  meta,
  icon,
  current,
  average,
  min,
  max,
  series,
}: KpiCardProps) {
  const values = series.map((s) => s.value)
  const direction = trend(values)
  const delta =
    values.length > 1 ? values[values.length - 1] - values[0] : 0

  const trendIcon =
    direction === 'up' ? (
      <ArrowUpRight size={14} />
    ) : direction === 'down' ? (
      <ArrowDownRight size={14} />
    ) : (
      <Minus size={14} />
    )

  const gradId = `grad-${meta.key}`

  return (
    <div className="kpi-card" style={{ ['--accent' as string]: meta.accent }}>
      <div className="kpi-header">
        <div className="kpi-icon">{icon}</div>
        <div className="kpi-titles">
          <div className="kpi-label">{meta.label}</div>
          <div className="kpi-desc">{meta.description}</div>
        </div>
        <span className={`kpi-trend trend-${direction}`}>
          {trendIcon}
          {formatNumber(Math.abs(delta), 1)}
        </span>
      </div>

      <div className="kpi-value">
        {formatNumber(current, 1)}
        <span className="kpi-unit">{meta.unit}</span>
      </div>

      <div className="kpi-sparkline">
        <ResponsiveContainer width="100%" height={60}>
          <AreaChart
            data={series}
            margin={{ top: 4, right: 0, bottom: 0, left: 0 }}
          >
            <defs>
              <linearGradient id={gradId} x1="0" y1="0" x2="0" y2="1">
                <stop offset="0%" stopColor={meta.accent} stopOpacity={0.5} />
                <stop offset="100%" stopColor={meta.accent} stopOpacity={0} />
              </linearGradient>
            </defs>
            <Tooltip
              content={<CustomTooltip />}
              cursor={{ stroke: meta.accent, strokeOpacity: 0.3 }}
            />
            <Area
              type="monotone"
              dataKey="value"
              stroke={meta.accent}
              strokeWidth={2}
              fill={`url(#${gradId})`}
              isAnimationActive
            />
          </AreaChart>
        </ResponsiveContainer>
      </div>

      <div className="kpi-stats">
        <div className="kpi-stat">
          <span>Ort</span>
          <strong>{formatNumber(average, 1)}</strong>
        </div>
        <div className="kpi-stat">
          <span>Min</span>
          <strong>{formatNumber(min, 1)}</strong>
        </div>
        <div className="kpi-stat">
          <span>Max</span>
          <strong>{formatNumber(max, 1)}</strong>
        </div>
      </div>
    </div>
  )
}
