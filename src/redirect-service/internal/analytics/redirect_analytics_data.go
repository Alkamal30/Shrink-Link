package analytics

import (
	"time"
)

type RedirectAnalyticsData struct {
	ShortCode string    `json:"short_code"`
	Timestamp time.Time `json:"timestamp"`
	Ip        string    `json:"ip"`
	UserAgent string    `json:"user_agent"`
	Referer   string    `json:"referer"`
	Language  string    `json:"language"`
}
